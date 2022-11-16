resource "aws_ecr_repository" "repository" {
  name                 = local.component_name
  image_tag_mutability = "IMMUTABLE"
}

data "aws_iam_policy_document" "repository" {
  statement {
    sid    = "push"
    effect = "Allow"
    actions = [
      "ecr:GetDownloadUrlForLayer",
      "ecr:BatchGetImage",
      "ecr:BatchCheckLayerAvailability",
      "ecr:BatchCheckLayerAvailability",
      "ecr:GetAuthorizationToken",
      "ecr:GetDownloadUrlForLayer",
      "ecr:PutImage",
      "ecr:InitiateLayerUpload",
      "ecr:UploadLayerPart",
      "ecr:CompleteLayerUpload"      
    ]
    principals {
      type        = "AWS"
      identifiers = [ var.container_publishing_role ]
    }
  }
}

resource "aws_ecr_repository_policy" "repository" {
  repository = aws_ecr_repository.repository.name
  policy     = data.aws_iam_policy_document.repository.json
}

resource "aws_ecr_lifecycle_policy" "lifecycle" {
  repository = aws_ecr_repository.repository.name

  policy = <<EOF
  {
    "rules": [
      { 
        "rulePriority": 1,
        "description": "Keep just 1 image.",
        "selection": {
          "tagStatus": "any",
          "countType": "imageCountMoreThan",
          "countNumber": 1
        },
        "action": {
          "type": "expire"
        }
      }
    ]
  }
  EOF
}