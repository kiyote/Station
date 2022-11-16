
data "aws_region" "current" {}

variable "object_prefix" {
  type = string
}

variable "service_prefix" {
  type = string
}

variable "container_publishing_role" {
  description = "The role to allow permission to push the container to the repository."
  type = string
}

variable "server_image" {
  description = "The tag to identify which container image is to be used."
  type = string
}

variable "image_prefix" {
  description = "The URI prefix used to identify containers within the registry."
  type = string
}