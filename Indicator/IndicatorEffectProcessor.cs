using System.Text;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using Vortice.Mathematics;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin;
using YukkuriMovieMaker.Settings;

namespace Indicator
{
    internal class IndicatorEffectProcessor : IVideoEffectProcessor
    {
        DisposeCollector disposer = new();

        IGraphicsDevicesAndContext devices;

        IndicatorEffect item;

        ID2D1Image? input;

        ID2D1CommandList? commandList;

        IDWriteFactory7 factory;

        public ID2D1Image Output => commandList ?? input;

        public IndicatorEffectProcessor(IGraphicsDevicesAndContext devices, IndicatorEffect item)
        {
            this.devices = devices;
            this.item = item;

            factory = DWrite.DWriteCreateFactory<IDWriteFactory7>();
            disposer.Collect(factory);
        }

        public void ClearInput()
        {
        }

        public void Dispose()
        {
            disposer.Dispose();
        }

        public void SetInput(ID2D1Image? input)
        {
            this.input = input;
        }

        public DrawDescription Update(EffectDescription effectDescription)
        {
            var frame = effectDescription.ItemPosition.Frame;
            var length = effectDescription.ItemDuration.Frame;
            var fps = effectDescription.FPS;

            var size = (float)item.Size.GetValue(frame, length, fps);
            var direction = item.Direction;
            var color = item.Color;
            var margin = (float)item.Margin.GetValue(frame, length, fps);
            var offsetX = (float)item.OffsetX.GetValue(frame, length, fps);
            var offsetY = (float)item.OffsetY.GetValue(frame, length, fps);
            var digit = $"F{(int)item.Digit.GetValue(frame, length, fps)}";

            var sb = new StringBuilder();

            var desc = effectDescription.DrawDescription;

            var bounds = devices.DeviceContext.GetImageLocalBounds(input);

            if (item.X)
            {
                sb.AppendLine($"X: {desc.Draw.X.ToString(digit)} px");
            }
            if (item.Y)
            {
                sb.AppendLine($"Y: {desc.Draw.Y.ToString(digit)} px");
            }
            if (item.Z)
            {
                sb.AppendLine($"Z: {desc.Draw.Z.ToString(digit)} px");
            }
            if (item.RotationX)
            {
                sb.AppendLine($"回転角X: {desc.Rotation.X.ToString(digit)}°");
            }
            if (item.RotationY)
            {
                sb.AppendLine($"回転角Y: {desc.Rotation.Y.ToString(digit)}°");
            }
            if (item.RotationZ)
            {
                sb.AppendLine($"回転角Z: {desc.Rotation.Z.ToString(digit)}°");
            }
            if (item.ZoomX)
            {
                sb.AppendLine($"拡大率X: {(desc.Zoom.X * 100).ToString(digit)} %");
            }
            if (item.ZoomY)
            {
                sb.AppendLine($"拡大率Y: {(desc.Zoom.Y * 100).ToString(digit)} %");
            }
            if (item.Opacity)
            {
                sb.AppendLine($"不透明度: {(desc.Opacity * 100).ToString(digit)} %");
            }
            if (item.Invert)
            {
                sb.AppendLine($"左右反転: {(desc.Invert ? "有効" : "無効")}");
            }
            if (item.InterpolationMode)
            {
                sb.AppendLine($"補間モード: {desc.ZoomInterpolationMode}");
            }
            if (item.Width)
            {
                sb.AppendLine($"幅: {(bounds.Right - bounds.Left).ToString(digit)} px");
            }
            if (item.Height)
            {
                sb.AppendLine($"高さ: {(bounds.Bottom - bounds.Top).ToString(digit)} px");
            }

            var text = sb.ToString();


            if (commandList is not null)
            {
                disposer.RemoveAndDispose(ref commandList);
            }

            var dc = devices.DeviceContext;
            commandList = dc.CreateCommandList();
            disposer.Collect(commandList);
            dc.Target = commandList;
            dc.BeginDraw();
            dc.Clear(null);
            dc.DrawImage(input);

            var font = SettingsBase<FontSettings>.Default.SystemFonts
                       .Concat(SettingsBase<FontSettings>.Default.CustomFonts)
                       .FirstOrDefault(f => f.FontName == item.Font)
                       ?? new Font();

            using var format = factory.CreateTextFormat(font.CanonicalFontName, null, (Vortice.DirectWrite.FontWeight)font.CanonicalFontWeight, (Vortice.DirectWrite.FontStyle)font.CanonicalFontStyle, (Vortice.DirectWrite.FontStretch)font.CanonicalFontStretch, Math.Max(1f, size));

            Rect rect;

            switch (direction)
            {
                case Direction.Bottom:
                    rect = new Rect(bounds.Left + offsetX, bounds.Bottom + offsetY + margin, float.MaxValue, float.MaxValue);
                    break;
                case Direction.Right:
                    rect = new Rect(bounds.Right + offsetX + margin, bounds.Top + offsetY, float.MaxValue, float.MaxValue);
                    break;
                default:
                    rect = new Rect(bounds.Left + offsetX, bounds.Bottom + offsetY + margin, float.MaxValue, float.MaxValue);
                    break;
            }

            using var brush = dc.CreateSolidColorBrush(new Color(color.R, color.G, color.B, color.A));

            dc.DrawText(text, format, rect, brush);
            dc.EndDraw();
            dc.Target = null;
            commandList.Close();

            return effectDescription.DrawDescription;
        }
    }
}