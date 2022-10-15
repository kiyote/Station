module "station" {
    source = "../../common"

    object_prefix = "station.dev."
    repo_token = var.repo_token
    amplify_stage = "DEVELOPMENT"
}

variable "repo_token" {
  description = "Carries the access token for the repository. This value comes from TF_VAR_repo_token in the GitHub Environment."
  type = string
  default = ""
  sensitive = true
  nullable = false
}

output "build_webhook" {
  value = module.station.amplify_webhook_url
}