module "amplify" {
	source = "../modules/amplify"

	object_prefix = var.object_prefix
	repo_token = var.repo_token
}