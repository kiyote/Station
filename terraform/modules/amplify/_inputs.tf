variable "object_prefix" {
  type = string
}

variable "repo_token" {
  type = string
  default = ""
  sensitive = true
  nullable = false
}    