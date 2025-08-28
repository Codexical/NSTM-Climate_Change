import socket
import cv2
import struct
import openni.openni2 as openni2
import numpy as np
import threading
import time
import json
import os

latest_frame = None
frame_lock = threading.Lock()
stop_event = threading.Event()


def camera_thread_func():
    global latest_frame
    dev = None
    depth_stream = None
    print("[Camera Thread] Starting up...")

    try:
        openni2.initialize()
        dev = openni2.Device.open_any()
        if dev is None:
            print("[Camera Thread] ERROR: No device found.")
            stop_event.set()
            return

        depth_stream = dev.create_depth_stream()
        # depth_stream.set_video_mode(openni2.VideoMode(pixelFormat=openni2.PIXEL_FORMAT_DEPTH_1_MM, resolutionX=640, resolutionY=480, fps=30))
        depth_stream.start()
        print(
            "[Camera Thread] Camera started successfully and is continuously refreshing frames."
        )

        while not stop_event.is_set():
            frame = depth_stream.read_frame()
            dpt = np.array(frame.get_buffer_as_uint16()).reshape(
                (frame.height, frame.width)
            )
            dpt = dpt[60:420, :]

            dpt = project_circle_to_center(dpt, 300, 190, 140)

            dpt = dpt[:, 140:500]

            dpt = cv2.resize(dpt, (50, 50), interpolation=cv2.INTER_NEAREST)

            dpt = np.flip(dpt, axis=0)
            dpt = np.flip(dpt, axis=1)
            # print(dpt[180, 320])
            # for i in range(24):
            #     for j in range(32):
            #         print(dpt[i, j], end=" ")
            #     print()

            with frame_lock:
                latest_frame = dpt.copy()

    except Exception as e:
        print(f"[Camera Thread] A critical error occurred: {e}")
    finally:
        print("[Camera Thread] Closing camera resources...")
        if depth_stream:
            depth_stream.stop()
        if dev:
            dev.close()
        openni2.unload()
        print("[Camera Thread] Stopped.")


def project_circle_to_center(original_array, center_x, center_y, radius):
    height, width = original_array.shape
    dtype = original_array.dtype
    new_center_x = width // 2
    new_center_y = height // 2
    new_radius = min(new_center_x, new_center_y)
    projected_array = np.zeros((height, width), dtype=dtype)
    y_coords, x_coords = np.indices((height, width))
    dist_from_new_center = np.sqrt(
        (x_coords - new_center_x) ** 2 + (y_coords - new_center_y) ** 2
    )
    mask = dist_from_new_center <= new_radius
    norm_x = (x_coords[mask] - new_center_x) / new_radius
    norm_y = (y_coords[mask] - new_center_y) / new_radius
    src_x_float = center_x + norm_x * radius
    src_y_float = center_y + norm_y * radius
    src_x_int = np.round(src_x_float).astype(int)
    src_y_int = np.round(src_y_float).astype(int)
    src_x_int = np.clip(src_x_int, 0, width - 1)
    src_y_int = np.clip(src_y_int, 0, height - 1)
    pixel_values = original_array[src_y_int, src_x_int]
    projected_array[mask] = pixel_values

    return projected_array


def handle_client_stream(conn, addr):
    global latest_frame
    print(f"[Stream Handler] Starting stream to {addr}.")

    try:
        while not stop_event.is_set():
            current_frame = None
            with frame_lock:
                if latest_frame is not None:
                    current_frame = latest_frame.copy()

            if current_frame is not None:
                data = current_frame.tobytes()
                message_size = struct.pack("Q", len(data))

                conn.sendall(message_size + data)

            time.sleep(0.03)

    except (ConnectionResetError, BrokenPipeError):
        print(f"[Stream Handler] Client {addr} has disconnected.")
    except Exception as e:
        print(f"[Stream Handler] Error while sending to {addr}: {e}")
    finally:
        print(f"[Stream Handler] Closing connection with {addr}.")
        conn.close()


def server_thread_func(host="0.0.0.0", port=8888):
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((host, port))
    server_socket.listen(5)
    server_socket.settimeout(1.0)
    print(f"[Server Thread] Listening on {host}:{port}")

    while not stop_event.is_set():
        try:
            conn, addr = server_socket.accept()
            client_handler = threading.Thread(
                target=handle_client_stream, args=(conn, addr), daemon=True
            )
            client_handler.start()
        except socket.timeout:
            continue
        except Exception as e:
            print(f"[Server Thread] An error occurred: {e}")
            break

    print("[Server Thread] Closing socket...")
    server_socket.close()
    print("[Server Thread] Stopped.")


if __name__ == "__main__":
    cam_thread = threading.Thread(target=camera_thread_func, daemon=True)
    serv_thread = threading.Thread(target=server_thread_func, daemon=True)
    cam_thread.start()
    serv_thread.start()
    print("[Main Program] Server started. Press Ctrl+C to stop.")
    try:
        cam_thread.join()
        serv_thread.join()
    except KeyboardInterrupt:
        print("\n[Main Program] Ctrl+C detected, preparing to shut down all threads...")
    finally:
        stop_event.set()
        print(
            "[Main Program] All threads have been closed successfully. Program finished."
        )
