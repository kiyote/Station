
output "amplify_webhook_url" {
    value = aws_amplify_webhook.main.url
    description = "The URL to invoke to cause an Amplify build."
}