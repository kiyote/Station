resource "aws_amplify_app" "station" {
    name = "Station"
    repository = var.source_repository
    access_token = var.repo_token
    iam_service_role_arn = aws_iam_role.webclient_role.arn
    platform = "WEB"

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
        - ./dotnet6/dotnet publish -c Release -o release
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
  branch_name = var.source_repository_branch
  framework = "Blazor"
  stage = var.webclient_stage
  enable_auto_build = false
}

resource "aws_amplify_webhook" "main" {
  app_id = aws_amplify_app.station.id
  branch_name = aws_amplify_branch.main.branch_name
}

data "aws_iam_policy_document" "amplify_assume_role_policy_document" {
    statement {
        effect = "Allow"
        actions = [
            "sts:AssumeRole"
        ]
        principals {
            type = "Service"
            identifiers = [ "amplify.amazonaws.com" ]
        }
    }
}

resource "aws_iam_role" "webclient_role" {
    name = "${var.object_prefix}webclient_role"

    assume_role_policy = data.aws_iam_policy_document.amplify_assume_role_policy_document.json
}

resource "aws_iam_role_policy_attachment" "amplify_administrator" {
    role = aws_iam_role.webclient_role.name
    policy_arn = "arn:aws:iam::aws:policy/AdministratorAccess-Amplify"
}
