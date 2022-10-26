
resource "aws_s3_bucket" "webclient" {
    bucket = "${var.object_prefix}webclient"

    versioning {
        enabled = false
    }
}

resource "aws_s3_bucket_website_configuration" "website" {
    bucket = aws_s3_bucket.webclient.bucket

    index_document {
        suffix = "index.html"
    }
}