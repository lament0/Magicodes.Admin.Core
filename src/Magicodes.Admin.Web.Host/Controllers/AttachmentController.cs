﻿using Abp.AspNetCore.Mvc.Controllers;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.IO.Extensions;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using Magicodes.Admin.IO;
using Magicodes.Storage;
using Magicodes.Storage.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Magicodes.App.Core.Attachments;

namespace Magicodes.Admin.Web.Controllers
{
    public class AttachmentController : AbpController
    {
        private readonly IRepository<AttachmentInfo, long> _attachmentInfoRepository;

        public AttachmentController(IRepository<AttachmentInfo, long> attachmentInfoRepository)
        {
            this._attachmentInfoRepository = attachmentInfoRepository;
        }

        public static IStorageProvider StorageProvider { get; set; }

        [HttpPost]
        [DisableAuditing]
        public async Task<ActionResult> UploadFiles()
        {
            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
            {
                throw new UserFriendlyException(L("File_Empty_Error"));
            }
            try
            {
                var filesOutput = new List<AttachmentInfo>();
                foreach (var item in files)
                {
                    if (item == null)
                    {
                        throw new UserFriendlyException(L("文件上传错误!"));
                    }
                    //5M
                    if (item.Length > 5242880)
                    {
                        throw new UserFriendlyException(L("File_SizeLimit_Error"));
                    }

                    try
                    {
                        using (var stream = item.OpenReadStream())
                        {
                            var tempFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(item.FileName);
                            await StorageProvider.SaveBlobStream((AbpSession.TenantId ?? 0).ToString(), tempFileName, stream);
                            var blobInfo = await StorageProvider.GetBlobFileInfo((AbpSession.TenantId ?? 0).ToString(), tempFileName);
                            var attachmentType = AttachmentTypes.File;
                            if (blobInfo.ContentType.StartsWith("video/"))
                            {
                                attachmentType = AttachmentTypes.Video;
                            }
                            else if (blobInfo.ContentType.StartsWith("image/"))
                            {
                                attachmentType = AttachmentTypes.Image;
                            }
                            else if (blobInfo.ContentType.StartsWith("audio/"))
                            {
                                attachmentType = AttachmentTypes.Audio;
                            }
                            var attach = new AttachmentInfo()
                            {
                                ContentType = blobInfo.ContentType,
                                CreationTime = Clock.Now,
                                CreatorUserId = AbpSession.UserId,
                                FileLength = blobInfo.Length,
                                Name = item.FileName,
                                TenantId = AbpSession.TenantId,
                                Url = blobInfo.Url,
                                BlobName = blobInfo.Name,
                                ContainerName = blobInfo.Container,
                                AttachmentType = attachmentType,
                                ContentMD5 = blobInfo.ContentMD5
                            };
                            _attachmentInfoRepository.Insert(attach);
                            filesOutput.Add(attach);
                        }
                    }
                    catch (StorageException ex)
                    {
                        throw new UserFriendlyException(message: ex.ProviderMessage);
                    }
                }
                return Json(new AjaxResponse(filesOutput));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
