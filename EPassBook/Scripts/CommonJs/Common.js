function ValidateOTP(installmentId, Otp) {
    debugger;
    var isOTPverfied = false;
    $.ajax({
        url: "/WorkFlow/ValidateOtp",
        type: "GET",
        data: { 'installmentID': installmentId, 'Otp': Otp },
        async: false,
        success: (function (data) {
            debugger;
            isOTPverfied = data;
        }),
        failure: (function (response) {
            alert('Try again Failed!');
        }),
        error: (function (xhr, textStatus, errorThrown) {
            if (textStatus != "error") { }
            else if (xhr.status === 401 || xhr.status === 403) { }
        })
    });

    return isOTPverfied;
}

function KeyPressEvent(e) {
    debugger;
   
            var a = [];
            var k = e.which;
            for (i = 48; i < 58; i++)
                a.push(i);
            if (!(a.indexOf(k) >= 0))
                e.preventDefault();

            var length = $("#txtOtp").val().length;
            if (length == 4) {
                $("#otpDiv").show();
                $('#btnSubmit').show();
            }

}

function AjaxWithParameter(InstallmentId) {
    $("#txtOtp").val('');
   
    $.ajax({
        url: "/WorkFlow/SendOtp",
        type: "GET",
        contentType: "application/json",
        //dataType: "json",
        data: { 'installmentID': InstallmentId },
        success: (function (response) {
            debugger;
            $("#otpDiv").show();
            $("#btnGenOTP").text('Re-Generate OTP');
            alert(response);
        }),
        failure: (function (response) {
            alert('Try again Failed!');
            return false;
        }),
        error: (function (xhr, textStatus, errorThrown) {
            if (textStatus != "error") {
            }
            else if (xhr.status === 401 || xhr.status === 403) {
            }
        })
    });
}