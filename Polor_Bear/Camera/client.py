import socket
import cv2
import struct
import numpy as np

HOST = "127.0.0.1"
PORT = 8888

FRAME_HEIGHT = 360
FRAME_WIDTH = 640


def receive_all(sock, n):
    data = bytearray()
    while len(data) < n:
        packet = sock.recv(n - len(data))
        if not packet:
            return None
        data.extend(packet)
    return data


def main():
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        print(f"[*] Connecting to {HOST}:{PORT}...")
        client_socket.connect((HOST, PORT))
        print("[*] Connected successfully.")
    except ConnectionRefusedError:
        print(f"[!] Connection failed. Is the server script running at {HOST}:{PORT}?")
        return
    except Exception as e:
        print(f"[!] An error occurred during connection: {e}")
        return

    payload_size = struct.calcsize("Q")

    try:
        while True:
            packed_msg_size = receive_all(client_socket, payload_size)
            if not packed_msg_size:
                print("[!] Server closed the connection.")
                break

            msg_size = struct.unpack("Q", packed_msg_size)[0]

            frame_data = receive_all(client_socket, msg_size)
            if not frame_data:
                print("[!] Server closed the connection unexpectedly.")
                break

            depth_frame = np.frombuffer(frame_data, dtype=np.uint16)
            depth_frame = depth_frame.reshape((FRAME_HEIGHT, FRAME_WIDTH))

            normalized_frame = cv2.normalize(
                depth_frame, None, 0, 255, cv2.NORM_MINMAX, dtype=cv2.CV_8U
            )

            colored_frame = cv2.applyColorMap(normalized_frame, cv2.COLORMAP_JET)

            cv2.imshow("Depth Stream from Server", colored_frame)

            if cv2.waitKey(1) & 0xFF == ord("q"):
                break

    except Exception as e:
        print(f"\n[!] An error occurred during streaming: {e}")
    finally:
        print("[*] Closing connection and cleaning up...")
        client_socket.close()
        cv2.destroyAllWindows()
        print("[*] Client shut down.")


if __name__ == "__main__":
    main()
