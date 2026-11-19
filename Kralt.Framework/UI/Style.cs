using Kralt.Mathematics;

namespace Kralt.Framework.UI
{
    public class Style
    {
        // PROPERTIES I WANT TO IMPLEMENT TODAY

        public Color? backgroundColor { get; set; }

        public Vector4? padding { get; set; }

        public Vector4? margin { get; set; }

        public float? width { get; set; }

        public float? height { get; set; }

        public float? maxHeight { get; set; }

        public float? maxWidth { get; set; }

        public float? minHeight { get; set; }

        public float? minWidth { get; set; }

        public float? flexGrow { get; set; }

        public Display? display { get; set; }

        public Alignment? alignItems { get; set; }

        public Alignment? alignSelf { get; set; }

        // LATER DOWN THE LINE:

        /*
        
        public Alignment? alignContent { get; set; }

        public float? flexShrink { get; set; }

        public float? aspectRatio { get; set; }

        public Color? borderColor { get; set; }

        public Vector4? borderWidth { get; set; }

        public Vector4? borderRadius { get; set; }

        public Color? color { get; set; }

        public CursorStyle? cursor { get; set; }

        public FlexDirection? flexDirection { get; set; }

        public WrapMode? flexWrap { get; set; }

        public float? fontSize { get; set; }

        public Justify? justifyContent { get; set; }

        public Overflow? overflow { get; set; }

        public Position? position { get; set; }

        public float? top { get; set; }

        public float? right { get; set; }

        public float? bottom { get; set; }

        public float? left { get; set; }

        public TextOverflow? textOverflow { get; set; }

        public Vector2? translate { get; set; }

        // public Font fontDefinition { get; set; }

        public OverflowClipBox? overflowClipBox { get; set; }

        public TextAnchor? textAlign { get; set; }

        */
    }

    #region Keyword Enums

    public enum Alignment
    {
        Auto,
        FlexStart,
        Center,
        FlexEnd,
        Stretch
    }
    
    public enum Display
    {
        Flex,
        None
    }
    /*
    public enum FlexDirection
    {
        Column,
        ColumnReverse,
        Row,
        RowReverse
    }

    public enum WrapMode
    {
        None,
        Wrap,
        WrapReverse
    }

    public enum Justify
    {
        FlexStart,
        Center,
        FlexEnd,
        SpaceBetween,
        SpaceAround,
        SpaceEvenly
    }

    public enum Overflow
    {
        Visible,
        Hidden
    }

    public enum Position
    {
        Relative,
        Absolute
    }

    public enum TextOverflow
    {
        Clip,
        Ellipsis
    }

    public enum FontStyle
    {
        Normal,
        Bold,
        Italic,
        BoldAndItalic
    }

    public enum OverflowClipBox
    {
        PaddingBox,
        ContentBox
    }

    public enum TextAnchor
    {
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerCenter,
        LowerRight
    }

    public enum CursorStyle
    {
        Default,
        Pointer,
        IBeam,
        MoveHorizontal,
        MoveVertical,
        Eyedropper,
        Grab
    }
    */
    #endregion
}
