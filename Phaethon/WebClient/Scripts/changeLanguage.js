$(function () {
    $('#ChangeLanguageEn').click(function () {
        ChangeLanguage("en");
    });
    $('#ChangeLanguageLv').click(function () {
        ChangeLanguage("lv");
    }); 
});

function ChangeLanguage(language)
{
    $.ajax({
        url: "/Language/ChangeLanguage?" +
            "&LanguageAbbreviation=" + language,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function () {
            window.location.reload();
        }
    });
};