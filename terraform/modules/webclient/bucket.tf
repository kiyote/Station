
resource "aws_s3_bucket" "bucket_webclient" {
    bucket = "${var.object_prefix}webclient"

    versioning {
        enabled = false
    }
}

resource "aws_s3_bucket_website_configuration" "bucket_website" {
    bucket = aws_s3_bucket.bucket_webclient.bucket

    index_document {
        suffix = "index.html"
    }
}

resource "aws_s3_bucket_acl" "bucket_acl" {
    bucket = aws_s3_bucket.bucket_webclient.bucket
    acl = "private"
}