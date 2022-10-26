variable "object_prefix" {
  type = string
}

variable "bucket_copy_role" {
  description = "The role to allow permission to copy files in to the bucket."
  type = string
}