﻿using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MemeTokenHub.Backoffce.Services
{
    public class S3UploadService : IUploadService
    {
        private readonly string? _bucketName;
        private readonly string? _accessKey;
        private readonly string? _secretKey;
        private readonly ILogger<S3UploadService> _logger;

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUWest2;
        public S3UploadService(IOptions<S3Settings> settings, ILogger<S3UploadService> logger)
        {
            _bucketName = settings.Value.BucketName;
            _accessKey = settings.Value.AccessKey;
            _secretKey = settings.Value.SecretKey;
            _logger = logger;

        }

        public async Task<bool> RemoveAsync(string memePageId, string section, string filename)
        {
            try
            {
                // Set up your AWS credentials
                var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
                using var s3Client = new AmazonS3Client(credentials, bucketRegion);

                var keyName = CreatePath(memePageId, section, filename);

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = keyName
                };

                var deleteObjectResponse = await s3Client.DeleteObjectAsync(deleteObjectRequest);

                return deleteObjectResponse.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
            return false;
        }

        public async Task<string> UploadAsync(string memePageId, string section, string fileName, Stream stream)
        {
            try
            {
                using (stream)
                {
                    // Set up your AWS credentials
                    var credentials = new BasicAWSCredentials(_accessKey, _secretKey);
                    using var s3Client = new AmazonS3Client(credentials, bucketRegion);
                    var fileTransferUtility = new TransferUtility(s3Client);

                    var key = CreatePath(memePageId, section, fileName);
                    await fileTransferUtility.UploadAsync(stream, _bucketName, key);
                    return CreateS3Url(bucketRegion, key);
                }
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }

            return null!;
        }

        private string CreateS3Url(RegionEndpoint bucketRegion, string key)
        {
            var region = bucketRegion.SystemName;

            return $"https://{_bucketName}.s3.{region}.amazonaws.com/{key}";
        }

        private static string CreatePath(string memepageId, string section, string filename)
        {
            return $"memepagemediaasset/{memepageId}/{section}/{filename}";
        }
    }
}
