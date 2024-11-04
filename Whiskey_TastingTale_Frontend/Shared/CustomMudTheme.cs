using MudBlazor;

public class CustomMudTheme : MudTheme
{
    public CustomMudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#D6C0B3", // 파란색
            Secondary = "#493628", // 녹색
            Background = "#E4E0E1", // 밝은 회색 배경
            AppbarBackground = "#AB886D", // 어두운 회색 앱바
            Surface = "#ffffff", // 흰색 표면
            TextPrimary = "#4A4947", // 기본 텍스트 색상 - 진한 회색
            TextSecondary = "#4A4947", // 보조 텍스트 색상 - 회색
            PrimaryContrastText = "#ffffff", 
            SecondaryContrastText = "#ffffff",

            ActionDefault = "#ffffff", // 액션 기본 색상 - 중간 회색
            ActionDisabled = "#adb5bd", // 비활성화된 액션 - 연한 회색
            DrawerBackground = "#ffffff", // 어두운 회색 서랍 배경
            DrawerText = "#4A4947", // 서랍 텍스트 색상

            Success = "#198754", // 성공 메시지 - 진한 녹색
            Error = "#dc3545", // 에러 메시지 - 진한 빨간색
            Warning = "#ffc107", // 경고 메시지 - 밝은 노란색
            Info = "#0dcaf0" // 정보 메시지 - 밝은 하늘색
        };

    }
}
