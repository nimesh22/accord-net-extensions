#region Licence and Terms
// Accord.NET Extensions Framework
// https://github.com/dajuric/accord-net-extensions
//
// Copyright © Darko Jurić, 2014-2015 
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Extensions;
using Accord.Extensions.Imaging;
using Point = AForge.IntPoint;
using PointF = AForge.Point;

namespace Accord.Extensions.Imaging.Algorithms.LINE2D
{
    /// <summary>
    /// Represents found LINE2D template.
    /// </summary>
    /// <typeparam name="T">LINE 2D template type.</typeparam>
    public class Match<T>
        where T: ITemplate
    {
        /// <summary>
        /// Gets or sets the X location.
        /// </summary>
        public int X;
        /// <summary>
        /// Gets or sets the Y location.
        /// </summary>
        public int Y;
        /// <summary>
        /// Gets or sets the matching score.
        /// </summary>
        public float Score;
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        public T Template;

        /// <summary>
        /// Gets the area of the match.
        /// </summary>
        public Rectangle BoundingRect
        {
            get { return GetBoundingRect(this as Match<ITemplate>); }
        }

        /// <summary>
        /// Gets the template features translated for match location.
        /// </summary>
        public IEnumerable<AForge.Point> Points
        {
            get 
            {
                return Template.Features
                       .Select(x => new AForge.Point(X + x.X, Y + x.Y));
            }
        }

        /// <summary>
        /// Gets the area of the match.
        /// </summary>
        /// <param name="m">Match.</param>
        /// <returns>The area of the match.</returns>
        public static Rectangle GetBoundingRect(Match<ITemplate> m)
        {
            Size size = m.Template.Size;
            return new Rectangle(m.X, m.Y, size.Width, size.Height);
        }

        /// <summary>
        /// Gets the center location of the match.
        /// </summary>
        /// <param name="m">Match.</param>
        /// <returns>The center point of the match.</returns>
        public static Point GetCenter(Match<ITemplate> m)
        {
            Rectangle matchRect = Match.GetBoundingRect(m);
            Point matchCenter = new Point(matchRect.X + matchRect.Width / 2, matchRect.Y + matchRect.Height / 2);
            return matchCenter;
        }
    }

    /// <summary>
    /// Represents found LINE2D template.
    /// </summary>
    public class Match: Match<ITemplate>
    {}

    /// <summary>
    /// Contains extension methods for match drawing.
    /// </summary>
    public static class ImageMatchExtensions
    {
        /// <summary>
        /// Draws a LINE2D match.
        /// </summary>
        /// <typeparam name="TColor">Image color.</typeparam>
        /// <typeparam name="TTemplate">LINE2D template type.</typeparam>
        /// <param name="image">Image.</param>
        /// <param name="match">Match to draw.</param>
        /// <param name="color">Color for the match.</param>
        /// <param name="thickness">Contour thickness.</param>
        /// <param name="drawOrientations">True to draw orientations. False to draw only template contour.</param>
        /// <param name="orientationColor">Feature orientation color.</param>
        public static void Draw<TColor, TTemplate>(this Image<TColor, Byte> image, Match<TTemplate> match, TColor color, int thickness = 3,
                                                   bool drawOrientations = false, TColor orientationColor = default(TColor))
            where TColor: IColor3
            where TTemplate: ITemplate
        {
            PointF offset = new PointF(match.X, match.Y);
            image.Draw(match.Template, offset, color, thickness, drawOrientations, orientationColor);
        }

        /// <summary>
        /// Draws a LINE2D match. 
        /// <para>The underlying template drawing function is default. To override use the function overload which takes LINE2D template type.</para>
        /// </summary>
        /// <typeparam name="TColor">Image color.</typeparam>
        /// <param name="image">Image.</param>
        /// <param name="match">Match to draw.</param>
        /// <param name="color">Color for the match.</param>
        /// <param name="thickness">Contour thickness.</param>
        /// <param name="drawOrientations">True to draw orientations. False to draw only template contour.</param>
        /// <param name="orientationColor">Feature orientation color.</param>
        public static void Draw<TColor>(this Image<TColor, Byte> image, Match match, TColor color, int thickness = 3,
                                        bool drawOrientations = false, TColor orientationColor = default(TColor))
            where TColor : IColor3
        {
            image.Draw<TColor, ITemplate>((Match<ITemplate>)match, color, thickness, drawOrientations, orientationColor);
        }
    }
}
