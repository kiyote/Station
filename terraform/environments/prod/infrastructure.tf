module "station" {
    source = "../../common"

    object_prefix = "station.prod."
    repo_token = var.repo_token
    amplify_stage = "PRODUCTION"
}

variable "repo_token" {
  description = "Carries the access token for the repository."
  type = string
  default = ""
  sensitive = true
  nullable = false
}