resource "aws_amplify_app" "station" {
    name = "Station"
    repository = "https://github.com/kiyote/Station"
    access_token = var.repo_token
    iam_service_role_arm = data.aws_iam_role.app_role.arn

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

    custom_rule {
        source = "</^[^.]+$|\\.(?!(css|gif|ico|jpg|js|png|txt|svg|woff|ttf|map|json|br|gz|html|md|eot|otf|dll|blat|wasm|dat)$)([^.]+$)/>"
        status = "200"
        target = "/index.html"
    }
}

resource "aws_amplify_branch" "main" {
    app_id = aws_amplify_app.station.id
    branch_name = "main"
    stage = "PRODUCTION"
    framework = "Blazor"
    enable_auto_build = true
}