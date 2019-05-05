using System;

using Maple.Domain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maple.Test
{
    public static class MediaItemModelContextModelExtensions
    {
        public static MediaItemModel CreateModelMediaItem(this TestContext context)
        {
            return new MediaItemModel()
            {
                CreatedBy = context.FullyQualifiedTestClassName,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = context.FullyQualifiedTestClassName,
                Description = $"Description for {context.FullyQualifiedTestClassName} single MediaItem",
                Duration = 0,
                Id = 1,
                IsDeleted = false,
                Location = "Memory",
                PrivacyStatus = (int)PrivacyStatus.None,
                Sequence = 1,
                Title = $"Title for {context.FullyQualifiedTestClassName} single MediaItem",
            };
        }
    }
}
