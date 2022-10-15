module "station" {
    source = "../../common"

    object_prefix = "station.dev."
    repo_token = var.repo_token
    amplify_stage = "DEVELOPMENT"
}

variable "repo_token" {
  description = "Carries the access token for the repository."
  type = string
  default = ""
  sensitive = true
  nullable = false
}

output "build_url" {
  value = module.station.amplify_webhook_url
}