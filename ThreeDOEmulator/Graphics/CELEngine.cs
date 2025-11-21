using System;
using ThreeDOEmulator.Memory;

namespace ThreeDOEmulator.Graphics
{
    /// <summary>
    /// 3DO CEL (Cel Animation) Graphics Engine
    /// Hardware-accelerated 2D sprite engine
    /// Supports 320x240, 640x480 resolutions at 16-bit color
    /// </summary>
    public class CELEngine
    {
        private MemoryBus _memory;
        private int _frameCount;

        // Display buffer (320x240 @ 16-bit = 153,600 bytes)
        private ushort[] _framebuffer;
        private int _width = 320;
        private int _height = 240;

        public CELEngine(MemoryBus memory)
        {
            _memory = memory;
            _framebuffer = new ushort[_width * _height];
            _frameCount = 0;

            Console.WriteLine($"CEL Engine initialized: {_width}x{_height} @ 16-bit color");
        }

        public void Update(int cycles)
        {
            // Graphics updates happen here
            // Rendering CELs, updating VRAM, etc.
        }

        public void RenderFrame()
        {
            _frameCount++;

            // Clear framebuffer (black)
            Array.Clear(_framebuffer, 0, _framebuffer.Length);

            // TODO: Render CELs from VRAM
            // TODO: Apply effects (scaling, rotation, transparency)

            // For now, just track frame count
            if (_frameCount % 60 == 0)
            {
                Console.WriteLine($"Rendered {_frameCount} frames");
            }
        }

        public void DrawCEL(int x, int y, ushort[] pixelData, int width, int height)
        {
            // Draw a CEL (sprite) to the framebuffer
            for (int py = 0; py < height; py++)
            {
                for (int px = 0; px < width; px++)
                {
                    int screenX = x + px;
                    int screenY = y + py;

                    if (screenX >= 0 && screenX < _width && screenY >= 0 && screenY < _height)
                    {
                        int fbIndex = screenY * _width + screenX;
                        int celIndex = py * width + px;

                        if (celIndex < pixelData.Length)
                        {
                            ushort pixel = pixelData[celIndex];

                            // Check for transparency (color 0)
                            if (pixel != 0)
                            {
                                _framebuffer[fbIndex] = pixel;
                            }
                        }
                    }
                }
            }
        }

        public ushort[] GetFramebuffer()
        {
            return _framebuffer;
        }
    }
}
