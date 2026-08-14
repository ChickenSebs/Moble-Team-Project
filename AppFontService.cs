using System;
using System.Drawing;

namespace calendar4
{
    public enum AppFontType
    {
        MalgunGothic = 0,
        Batang = 1,
        Dotum = 2,

        // 프리미엄
        HancomMalang = 3,
        HunminHorizontal = 4,
        HancomSanzDotum = 5
    }

    public static class AppFontService
    {
        // 현재 선택된 글꼴
        public static AppFontType CurrentFont { get; private set; }
            = AppFontType.MalgunGothic;


        // ============================================================
        // 현재 글꼴 변경
        // ============================================================
        public static void SetFont(AppFontType font)
        {
            CurrentFont = font;
        }


        // ============================================================
        // 현재 글꼴 이름 반환
        // ============================================================
        public static string GetFontName()
        {
            return GetFontName(CurrentFont);
        }


        // ============================================================
        // 글꼴별 실제 Windows 등록 이름 반환
        // ============================================================
        public static string GetFontName(AppFontType font)
        {
            return font switch
            {
                AppFontType.Batang =>
                    "바탕",

                AppFontType.Dotum =>
                    "돋움",

                // ★ 수정
                // 말랑말랑은 Regular와 Bold가
                // 별개의 FontFamily로 설치되어 있음
                AppFontType.HancomMalang =>
                    "한컴 말랑말랑 Regular",

                // ★ 실제 Windows 등록 이름
                AppFontType.HunminHorizontal =>
                    "한컴 훈민정음 가로쓰기",

                // ★ 실제 이름에는 한컴과 산뜻돋움 사이에 공백이 없음
                AppFontType.HancomSanzDotum =>
                    "한컴산뜻돋움",

                _ =>
                    "맑은 고딕"
            };
        }


        // ============================================================
        // 프리미엄 글꼴 여부
        // ============================================================
        public static bool IsPremiumFont(AppFontType font)
        {
            return font switch
            {
                AppFontType.HancomMalang => true,
                AppFontType.HunminHorizontal => true,
                AppFontType.HancomSanzDotum => true,

                _ => false
            };
        }


        // ============================================================
        // Font 객체 생성
        // ============================================================
        public static Font CreateFont(
            float size,
            FontStyle style = FontStyle.Regular)
        {
            try
            {
                // ====================================================
                // ★ 한컴 말랑말랑 특별 처리
                // ====================================================
                // 이 글꼴은
                //
                // 한컴 말랑말랑 Regular
                // 한컴 말랑말랑 Bold
                //
                // 가 서로 다른 FontFamily로 설치되어 있기 때문에
                // FontStyle.Bold 방식으로 처리하지 않고
                // 실제 Bold Family를 직접 선택함
                // ====================================================

                if (CurrentFont == AppFontType.HancomMalang)
                {
                    bool wantsBold =
                        (style & FontStyle.Bold) == FontStyle.Bold;

                    string malangFontName =
                        wantsBold
                            ? "한컴 말랑말랑 Bold"
                            : "한컴 말랑말랑 Regular";

                    FontFamily malangFamily =
                        new FontFamily(malangFontName);

                    // Family 이름 자체가 이미 Bold / Regular를
                    // 구분하므로 Regular 스타일로 생성
                    return new Font(
                        malangFamily,
                        size,
                        FontStyle.Regular);
                }


                // ====================================================
                // 나머지 글꼴
                // ====================================================

                string fontName =
                    GetFontName();

                FontFamily family =
                    new FontFamily(fontName);


                // 원래 컨트롤이 사용하던 스타일 지원
                if (family.IsStyleAvailable(style))
                {
                    return new Font(
                        family,
                        size,
                        style);
                }


                // Bold를 요청했는데 정확한 스타일이 없다면
                // Bold 사용 가능한지 확인
                if ((style & FontStyle.Bold) == FontStyle.Bold &&
                    family.IsStyleAvailable(FontStyle.Bold))
                {
                    return new Font(
                        family,
                        size,
                        FontStyle.Bold);
                }


                // Regular 사용 가능
                if (family.IsStyleAvailable(FontStyle.Regular))
                {
                    return new Font(
                        family,
                        size,
                        FontStyle.Regular);
                }


                // Italic 사용 가능
                if (family.IsStyleAvailable(FontStyle.Italic))
                {
                    return new Font(
                        family,
                        size,
                        FontStyle.Italic);
                }
            }
            catch
            {
                // ====================================================
                // 글꼴이 설치되지 않은 PC에서는
                // 프로그램을 종료시키지 않고 맑은 고딕 사용
                // ====================================================
            }


            // ========================================================
            // 최종 안전장치
            // ========================================================

            try
            {
                return new Font(
                    "맑은 고딕",
                    size,
                    style);
            }
            catch
            {
                return new Font(
                    "맑은 고딕",
                    size,
                    FontStyle.Regular);
            }
        }
    }
}