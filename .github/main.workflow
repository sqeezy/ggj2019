workflow "Build on push" {
  on = "push"
  resolves = ["GitHub Action for Docker"]
}

action "GitHub Action for Docker" {
  uses = "actions/docker/cli@c08a5fc9e0286844156fefff2c141072048141f6"
  secrets = ["UNITY_USERNAME", "UNITY_PASSWORD"]
  env = {
    BUILD_NAME = "GGJ2019-Alone"
    BUILD_TARGET = "WebGl"
    IMAGE_NAME = "gableroux/unity3d:2018.3.2f1"
  }
  runs = "docker run -e BUILD_NAME -e UNITY_LICENSE_CONTENT -e BUILD_TARGET -e UNITY_USERNAME -e UNITY_PASSWORD -w /project/  -v $(pwd):/project/ $IMAGE_NAME /bin/bash -c \"/project/ci/before_script.sh && /project/ci/build.sh\""
}
