﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetGame
{
    /// <summary>
    ///     Defines the look-up for Pet Images by ID.
    ///     We don't expect the paths of images to change at all
    ///     so it's totally fine in this case to hard code them.
    /// </summary>
    public static class PetImages
    {
        /// <summary>
        ///     Gets the default Image url.
        /// </summary>
        public static (string Path, string MIME) DefaultImage
            => (Path: "~/img/pets/mysterypet.png", MIME: "image/png");

        /// <summary>
        ///     Gets the number of images.
        /// </summary>
        public static int Count
            => Images.Count;

        private static Random rng = new Random();

        /// <summary>
        ///     Gets a random image id.
        /// </summary>
        public static int GetRandomId
            => rng.Next(Count);

        private static Dictionary<int, (string Path, string MIME)> Images
            => new Dictionary<int, (string Path, string MIME)>()
            {
                { 0, (Path: "~/img/pets/default.jpg", MIME: "image/jpeg") },
                { 1, (Path: "~/img/pets/pet_01.png", MIME: "image/png") },
                { 2, (Path: "~/img/pets/pet_02.png", MIME: "image/png") },
                { 3, (Path: "~/img/pets/pet_03.png", MIME: "image/png")},
                { 4, (Path: "~/img/pets/pet_04.png", MIME: "image/png") },
                { 5, (Path: "~/img/pets/pet_05.png", MIME: "image/png") },
                { 6, (Path: "~/img/pets/pet_06.png", MIME: "image/png") },
                { 7, (Path: "~/img/pets/pet_07.png", MIME: "image/png") },
                { 8, (Path: "~/img/pets/pet_08.png", MIME: "image/png") },
                { 9, (Path: "~/img/pets/pet_09.png", MIME: "image/png") },
                { 10, (Path: "~/img/pets/pet_10.png", MIME: "image/png") },
                { 11, (Path: "~/img/pets/pet_11.png", MIME: "image/png") },
                { 12, (Path: "~/img/pets/pet_12.png", MIME: "image/png") },
                { 13, (Path: "~/img/pets/pet_13.png", MIME: "image/png") },
                { 14, (Path: "~/img/pets/pet_14.jpg", MIME: "image/jpeg") },
                { 15, (Path: "~/img/pets/pet_15.png", MIME: "image/png") },
                { 16, (Path: "~/img/pets/benjamin.png", MIME: "image/png") },
                { 17, (Path: "~/img/pets/sadcat.png", MIME: "image/png") },
                { 18, (Path: "~/img/pets/pet_16.png", MIME: "image/png") },
                { 19, (Path: "~/img/pets/loops.png", MIME: "image/png") },
                { 20, (Path: "~/img/pets/fast.png", MIME: "image/png") },
            };

        public const int ImageGoldOrangId = 1001;
        public const int ImageHacker = 1002;
        public const int ImageThinking = 1003;
        public const int ImageLegs = 1003;
        public const int ImageVim = 1004;
        public const int ImageRasinBran = 1005;

        private static Dictionary<int, (string Path, string MIME)> SecretImages
            => new Dictionary<int, (string Path, string MIME)>()
            {
                { ImageGoldOrangId, (Path: "~/img/pets/ORANGOLD_small.png", MIME: "image/png") },
                { ImageHacker, (Path: "~/img/pets/hacker.png", MIME: "image/png") },
                { ImageThinking, (Path: "~/img/pets/thinking.png", MIME: "image/png") },
                { ImageLegs, (Path: "~/img/pets/legs.png", MIME: "image/png") },
                { ImageVim, (Path: "~/img/pets/vim.png", MIME: "image/png") },
                { ImageRasinBran, (Path: "~/img/pets/rasinbran.png", MIME: "image/png") },
            };

        /// <summary>
        ///     Gets Pet images by their ID. Defaults to the DefaultImage if not found.
        /// </summary>
        /// <param name="imageId">
        ///     The ID of the Pet Image to find.
        /// </param>
        /// <returns>
        ///     A url path of where the image is located (under wwwroot).
        ///     Tuple of the path and the mime type.
        /// </returns>
        public static (string Path, string MIME) GetImageById(int imageId)
        {
            // get the image path if it exists in the database
            if (Images.ContainsKey(imageId))
                return Images[imageId];
            if (SecretImages.ContainsKey(imageId))
                return SecretImages[imageId];
            return DefaultImage;
        }
    }
}
