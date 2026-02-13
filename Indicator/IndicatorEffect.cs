using System.ComponentModel.DataAnnotations;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Exo;
using YukkuriMovieMaker.Player.Video;
using YukkuriMovieMaker.Plugin.Effects;
using YukkuriMovieMaker.Resources.Localization;

namespace Indicator
{
    [VideoEffect("Indicator", [], [], isAviUtlSupported: false, isEffectItemSupported: false)]
    internal class IndicatorEffect : VideoEffectBase
    {
        public override string Label => "Indicator";

        [Display(GroupName = "テキスト", Name = "フォント", Description = "フォント")]
        [FontComboBox]
        public string Font { get => font; set => Set(ref font, value); }
        string font = Texts.DefaultFont;

        [Display(GroupName = "テキスト", Name = "サイズ", Description = "サイズ")]
        [AnimationSlider("F1", "px", 1, 50)]
        public Animation Size { get; } = new Animation(34, 1, 100000);

        [Display(GroupName = "テキスト", Name = "配置", Description = "配置")]
        [EnumComboBox]
        public Direction Direction { get => direction; set => Set(ref direction, value); }
        Direction direction = Direction.Bottom;

        [Display(GroupName = "テキスト", Name = "余白", Description = "余白")]
        [AnimationSlider("F1", "px", -50, 50)]
        public Animation Margin { get; } = new Animation(0, -100000, 100000);

        [Display(GroupName = "テキスト", Name = "オフセットX", Description = "オフセットX")]
        [AnimationSlider("F1", "px", -100, 100)]
        public Animation OffsetX { get; } = new Animation(0, -100000, 100000);

        [Display(GroupName = "テキスト", Name = "オフセットY", Description = "オフセットY")]
        [AnimationSlider("F1", "px", -100, 100)]
        public Animation OffsetY { get; } = new Animation(0, -100000, 100000);

        [Display(GroupName = "テキスト", Name = "色", Description = "色")]
        [ColorPicker]
        public System.Windows.Media.Color Color { get => color; set => Set(ref color, value); }
        System.Windows.Media.Color color = System.Windows.Media.Colors.White;

        [Display(GroupName = "テキスト", Name = "小数桁", Description = "小数桁")]
        [AnimationSlider("F0", "", 0, 5)]
        public Animation Digit { get; } = new Animation(2, 0, 5);

        [Display(GroupName = "要素", Name = "X", Description = "X")]
        [ToggleSlider]
        public bool X { get => x; set => Set(ref x, value); }
        bool x = false;

        [Display(GroupName = "要素", Name = "Y", Description = "Y")]
        [ToggleSlider]
        public bool Y { get => y; set => Set(ref y, value); }
        bool y = false;

        [Display(GroupName = "要素", Name = "Z", Description = "Z")]
        [ToggleSlider]
        public bool Z { get => z; set => Set(ref z, value); }
        bool z = false;

        [Display(GroupName = "要素", Name = "回転角X", Description = "回転角X")]
        [ToggleSlider]
        public bool RotationX { get => rotationX; set => Set(ref rotationX, value); }
        bool rotationX = false;

        [Display(GroupName = "要素", Name = "回転角Y", Description = "回転角Y")]
        [ToggleSlider]
        public bool RotationY { get => rotationY; set => Set(ref rotationY, value); }
        bool rotationY = false;

        [Display(GroupName = "要素", Name = "回転角Z", Description = "回転角Z")]
        [ToggleSlider]
        public bool RotationZ { get => rotationZ; set => Set(ref rotationZ, value); }
        bool rotationZ = false;

        [Display(GroupName = "要素", Name = "拡大率X", Description = "拡大率X")]
        [ToggleSlider]
        public bool ZoomX { get => zoomX; set => Set(ref zoomX, value); }
        bool zoomX = false;

        [Display(GroupName = "要素", Name = "拡大率Y", Description = "拡大率Y")]
        [ToggleSlider]
        public bool ZoomY { get => zoomY; set => Set(ref zoomY, value); }
        bool zoomY = false;

        [Display(GroupName = "要素", Name = "不透明度", Description = "不透明度")]
        [ToggleSlider]
        public bool Opacity { get => opacity; set => Set(ref opacity, value); }
        bool opacity = false;
        
        [Display(GroupName = "要素", Name = "左右反転", Description = "左右反転")]
        [ToggleSlider]
        public bool Invert { get => invert; set => Set(ref invert, value); }
        bool invert = false;

        [Display(GroupName = "要素", Name = "補間モード", Description = "補間モード")]
        [ToggleSlider]
        public bool InterpolationMode { get => interpolationMode; set => Set(ref interpolationMode, value); }
        bool interpolationMode = false;

        [Display(GroupName = "要素", Name = "幅", Description = "幅")]
        [ToggleSlider]
        public bool Width { get => width; set => Set(ref width, value); }
        bool width = false;

        [Display(GroupName = "要素", Name = "高さ", Description = "高さ")]
        [ToggleSlider]
        public bool Height { get => height; set => Set(ref height, value); }
        bool height = false;

        public override IEnumerable<string> CreateExoVideoFilters(int keyFrameIndex, ExoOutputDescription exoOutputDescription)
        {
            return [];
        }

        public override IVideoEffectProcessor CreateVideoEffect(IGraphicsDevicesAndContext devices)
        {
            return new IndicatorEffectProcessor(devices, this);
        }

        protected override IEnumerable<IAnimatable> GetAnimatables() => [Size, Margin, OffsetX, OffsetY, Digit];
    }
}
