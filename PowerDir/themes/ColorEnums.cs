namespace PowerDir.themes
{
    /// TODO consider instead to create classes instead?


    /// <summary>
    /// Support Bold/Dim as an extra parameter
    /// The ones listed are foreground color
    /// Background colors add 10 to their value
    /// The Dark Colors with Bold property to true becomes Bright colors
    /// </summary>
    enum Color16
    {
        Original = 39,
        Black = 30,
        DarkRed = 31,
        DarkGreen = 32,
        DarkYellow = 33,
        DarkBlue = 34,
        DarkMagenta = 35,
        DarkCyan = 36,
        Gray = 37,
        DarkGray = 90,
        Red = 91,
        Green = 92,
        Yellow = 93,
        Blue = 94,
        Magenta = 95,
        Cyan = 96,
        White = 97,
    }

    enum Color256
    {
        Original = -1,
        Black = 0,
        DarkRed = 1,
        DargGreen = 2,
        DarkYellow = 3,
        DarkBlue = 4,
        DarkMagenta = 5,
        DarkCyan = 6,
        Gray = 7,
        DarkGray = 8,
        Red = 9,
        Green = 10,
        Yellow = 11,
        Blue = 12,
        Magenta = 13,
        Cyan = 14,
        White = 15,
    }
    enum ColorRGB
    {
        Original = -1,
        Black = 0,
        DarkRed = 0x800000,
        DargGreen = 0x008000,
        DarkYellow = 0x808000,
        DarkBlue = 0x000080,
        DarkMagenta = 0x8000f0,
        DarkCyan = 0x008080,
        Gray = 0xf0f0f0,
        DarkGray = 0x808080,
        Red = 0xff0000,
        Green = 0x00ff00,
        Yellow = 0xffff00,
        Blue = 0x0000ff,
        Magenta = 0xff00ff,
        Cyan = 0x00ffff,
        White = 0xffffff,
    }
}
