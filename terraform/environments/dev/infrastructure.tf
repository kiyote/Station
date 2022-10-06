module "station" {
    source = "../../common"

    object_prefix = "station.dev."
    repo_token = var.repo_token
}

variable "repo_token" {
  type = string
  default = ""
  sensitive = true
  nullable = false
}
