using System;
using System.ComponentModel.DataAnnotations;

namespace VoiceChattingManagementServer.Core.Database.Attributes
{
    public class ColumnAttribute : Attribute
    {
        // https://velog.io/@gillog/JPA-Column-Annotation

        /// <summary>
        /// Column name
        /// </summary>
        [Required]
        public string Name { get; private set; }
        /// <summary>
        /// Can insert value to table
        /// </summary>
        public bool Insertable { get; private set; } = true;
        /// <summary>
        /// Can update value to table
        /// </summary>
        public bool Updatable { get; private set; } = true;
        /// <summary>
        /// Value can be null
        /// </summary>
        public bool Nullable { get; private set; } = true;
        /// <summary>
        /// Uniqure value
        /// </summary>
        public bool Unique { get; private set; } = false;
        /// <summary>
        /// Data max length
        /// </summary>
        public int Length { get; private set; } = 255;

        /// <summary>
        /// Define table column
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="insertable">Can insert value to table</param>
        /// <param name="updatable">Can update value to table</param>
        /// <param name="nullable">Value can be null</param>
        /// <param name="unique">Uniqure value</param>
        /// <param name="length">Data max length</param>
        public ColumnAttribute(
            string name,
            bool insertable = true,
            bool updatable = true,
            bool nullable = true,
            bool unique = false,
            int length = 255)
        {
            Name = name;
            Insertable = insertable;
            Updatable = updatable;
            Nullable = nullable;
            Unique = unique;
            Length = length;
        }
    }
}
