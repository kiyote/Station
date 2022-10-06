
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

resource "aws_iam_role" "app_role" {
    name = "${var.object_prefix}app_role"

    assume_role_policy = data.aws_iam_policy_document.amplify_assume_role_policy_document.json
}

resource "aws_iam_role_policy_attachment" "amplify_administrator" {
    role = aws_iam_role.app_role.name
    policy_arn = "arn:aws:iam::aws:policy/AdministratorAccess-Amplify"
}
