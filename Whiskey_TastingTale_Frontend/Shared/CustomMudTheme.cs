using MudBlazor;

public class CustomMudTheme : MudTheme
{
    public CustomMudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#D6C0B3", 
            Secondary = "#493628", 
            Background = "#E4E0E1", 
            AppbarBackground = "#AB886D", 
            Surface = "#ffffff", // 흰색 표면
            TextPrimary = "#4A4947", // 기본 텍스트 색상 - 진한 회색
            TextSecondary = "#4A4947", // 보조 텍스트 색상 - 회색
            PrimaryContrastText = "#ffffff",
            SecondaryContrastText = "#ffffff",

            ActionDefault = "#4A4947", // 액션 기본 색상 - 중간 회색
            ActionDisabled = "#adb5bd", // 비활성화된 액션 - 연한 회색
            DrawerBackground = "#ffffff", // 어두운 회색 서랍 배경
            DrawerText = "#4A4947", // 서랍 텍스트 색상

            Success = "#198754", // 성공 메시지 - 진한 녹색
            Error = "#dc3545", // 에러 메시지 - 진한 빨간색
            Warning = "#ffc107", // 경고 메시지 - 밝은 노란색
            Info = "#0dcaf0", // 정보 메시지 - 밝은 하늘색
            Divider = "#4A4947",
        };
        PaletteDark = new()
        {
            Primary = "#7e6fff",
            Surface = "#1e1e2d",
            Background = "#1a1a27",
            BackgroundGray = "#151521",
            AppbarText = "#92929f",
            AppbarBackground = "rgba(26,26,39,0.8)",
            DrawerBackground = "#1a1a27",
            ActionDefault = "#74718e",
            ActionDisabled = "#9999994d",
            ActionDisabledBackground = "#605f6d4d",
            TextPrimary = "#b2b0bf",
            TextSecondary = "#92929f",
            TextDisabled = "#ffffff33",
            DrawerIcon = "#92929f",
            DrawerText = "#92929f",
            GrayLight = "#2a2833",
            GrayLighter = "#1e1e2d",
            Info = "#4a86ff",
            Success = "#3dcb6c",
            Warning = "#ffb545",
            Error = "#ff3f5f",
            LinesDefault = "#33323e",
            TableLines = "#33323e",
            Divider = "#292838",
            OverlayLight = "#1e1e2d80",
        };
        LayoutProperties = new LayoutProperties();
    }
}
