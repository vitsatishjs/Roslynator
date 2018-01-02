// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class ImageTests
    {
        [Fact]
        public void Image_Equals()
        {
            Image image = CreateImage();

            Assert.True(image.Equals((object)image));
        }

        [Fact]
        public void Image_NotEquals()
        {
            Image image = CreateImage();
            Image image2 = image.Modify();

            Assert.False(image.Equals((object)image2));
        }

        [Fact]
        public void Image_IEquatableEquals()
        {
            Image image = CreateImage();
            Image image2 = image;
            IEquatable<Image> equatable = image;

            Assert.True(equatable.Equals(image2));
        }

        [Fact]
        public void Image_IEquatableNotEquals()
        {
            Image image = CreateImage();
            Image image2 = CreateImage().Modify();
            IEquatable<Image> equatable = image;

            Assert.False(image.Equals(image2));
        }

        [Fact]
        public void Image_GetHashCode_Equal()
        {
            Image image = CreateImage();

            Assert.Equal(image.GetHashCode(), image.GetHashCode());
        }

        [Fact]
        public void Image_GetHashCode_NotEqual()
        {
            Image image = CreateImage();
            Image image2 = image.Modify();

            Assert.NotEqual(image.GetHashCode(), image2.GetHashCode());
        }

        [Fact]
        public void Image_OperatorEquals()
        {
            Image image = CreateImage();
            Image image2 = image;

            Assert.True(image == image2);
        }

        [Fact]
        public void Image_OperatorNotEquals()
        {
            Image image = CreateImage();
            Image image2 = image.Modify();

            Assert.True(image != image2);
        }

        [Fact]
        public void Image_Constructor_AssignText()
        {
            string text = LinkText();
            var image = new Image(text: text, url: LinkUrl(), title: LinkTitle());

            Assert.Equal(text, image.Text);
        }

        [Fact]
        public void Image_WithText()
        {
            string text = LinkText();

            Assert.Equal(text, CreateImage().WithText(text).Text);
        }

        [Fact]
        public void Image_Constructor_AssignUrl()
        {
            string url = LinkUrl();
            var image = new Image(text: LinkText(), url: url, title: LinkTitle());

            Assert.Equal(url, image.Url);
        }

        [Fact]
        public void Image_WithUrl()
        {
            Image image = CreateImage();
            string url = image.Url.Modify();

            Assert.Equal(url, image.WithUrl(url).Url);
        }

        [Fact]
        public void Image_Constructor_AssignTitle()
        {
            string title = LinkTitle();
            var image = new Image(text: LinkText(), url: LinkUrl(), title: title);

            Assert.Equal(title, image.Title);
        }

        [Fact]
        public void Image_WithTitle()
        {
            Image image = CreateImage();
            string title = image.Title.Modify();

            Assert.Equal(title, image.WithTitle(title).Title);
        }
    }
}
