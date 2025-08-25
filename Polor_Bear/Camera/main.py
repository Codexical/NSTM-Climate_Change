import subprocess
import time
import sys

TARGET_SCRIPT = "camera.py"


def main():
    while True:
        process = subprocess.Popen([sys.executable, TARGET_SCRIPT])
        process.wait()

        if process.returncode != 0:
            time.sleep(1)
        else:
            break


if __name__ == "__main__":
    main()
