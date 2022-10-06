resource "aws_amplify_app" "station" {
    name = "Station"
    repository = "https://github.com/kiyote/Station"
    access_token = var.repo_token

    build_spec = <<-EOT
        version: 1
        frontend:
        phases:
        preBuild:
        commands:
        - curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
        - chmod +x *.sh
        - ./dotnet-install.sh -c 6.0 -InstallDir ./dotnet6
        - ./dotnet6/dotnet --version
        build:
        commands:
        - ./dotnet6/dotnet publish -c Release -o releas
        artifacts:
        baseDirectory: /release/wwwroot
        files:
        - '**/*'
        cache:
        paths: []
    EOT
}