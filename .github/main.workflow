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
  runs = "chmod +x ./ci/docker_build.sh"
}
