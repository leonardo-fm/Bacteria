using System;
using System.Collections.Generic;
using System.Drawing;

namespace Bacteria;

public class DrawManager
{
    public event EventHandler<Bitmap> OnFrameDrawn;

    private Bitmap bitmap;

    private Color bgColor;
    
    public DrawManager(int bitmapX, int bitmapY, Color bgColor)
    {
        bitmap = new Bitmap(bitmapX, bitmapY);
        this.bgColor = bgColor;
        ResetBitmap();
    }

    public void DrawFrame(List<(int x, int y, Color color)> colorVector)
    {
        foreach ((int x, int y, Color color) in colorVector)
            bitmap.SetPixel(x, y, color);
        
        OnFrameDrawn?.Invoke(this, new Bitmap(bitmap));
    }

    public Bitmap GetBitmap() => bitmap;

    public void ResetBitmap()
    {
        for (int x = 0; x < bitmap.Width - 1; x++)
            for (int y = 0; y < bitmap.Height - 1; y++)
                bitmap.SetPixel(x, y, bgColor);
    }
}
