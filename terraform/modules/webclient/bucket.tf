
resource "aws_s3_bucket" "bucket_webclient" {
   bucket = "${var.object_prefix}webclient"
}

resource "aws_s3_bucket_versioning" "bucket_versioning" {
  bucket = aws_s3_bucket.bucket_webclient.bucket

  versioning_configuration {
    status = "Disabled"
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

resource "aws_s3_bucket_public_access_block" "bucket_public" {
  bucket = aws_s3_bucket.bucket_webclient.bucket

  block_public_acls = false
  block_public_policy = false
  ignore_public_acls = false
  restrict_public_buckets = false
}

# The policy applied to the storage bucket
data "aws_iam_policy_document" "bucket_policy" {

  statement {
    effect = "Allow"
    actions = [
      "s3:GetObject"
    ]
    resources = [
      "${aws_s3_bucket.bucket_webclient.arn}/*",
    ]
     principals {
      type = "*"
      identifiers = [
        "*"
      ]
    }
  }

  statement {
    effect = "Allow"
    actions = [
      "s3:PutObject"
    ]
    resources = [
      "${aws_s3_bucket.bucket_webclient.arn}/*",
      aws_s3_bucket.bucket_webclient.arn,
    ]
    principals {
      type = "AWS"
      identifiers = [
        var.bucket_copy_role
      ]
    }
  }
}

# Associates the above policy with the storage bucket
resource "aws_s3_bucket_policy" "policy" {
  bucket = aws_s3_bucket.bucket_webclient.bucket
  policy = data.aws_iam_policy_document.bucket_policy.json
}