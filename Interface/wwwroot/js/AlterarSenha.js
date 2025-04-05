$.validator.addMethod("pwcheck",
    function (value) {
        //return /^[A-Za-z0-9\d=!\-@@._*]+$/.test(value);
        var crk1 = /[a-z]/.test(value);
        var crk2 = /[A-Z]/.test(value);
        var crk3 = /\d/.test(value);
        var crk4 = /[\!\@@\#\$\%\^\&\*\(\)\-\=\¡\£\_\+\`\~\.\,\<\>\/\?\;\:\'\"\\\|\[\]\{\}]/.test(value);
        return crk1 && crk2 && crk3 && crk4;
    });
$(function () {
    $.validator.setDefaults({
        submitHandler: function () {
            alert("Formulário enviado com sucesso!");
        }
    });
    $('#quickForm').validate({
        rules: {
            password: {
                required: true,
                pwcheck: true,
                minlength: 6
            },
            confirmPassword: {
                required: true,
                minlength: 6,
                equalTo: "#password"
            }
        },
        messages: {
            password: {
                required: "Forneça uma senha",
                pwcheck: "A senha não satisfaz os critérios!\r\nA sua senha deve ter pelo menos 6 caracteres\r\npelo menos uma letra minúscula\r\npelo menos um número\r\npelo menos um caracter especial: !@@#$%^\u0026*()-=¡£_+`~.,\u003c\u003e/?;:\u0027\"|[]{}",
                minlength: "A sua senha deve ter pelo menos 6 caracteres"
            },
            confirmPassword: {
                required: "Forneça uma confirmação de senha",
                minlength: "A sua confirmação de senha deve ter pelo menos 6 caracteres",
                equalTo: "A confirmação da senha tem que ser idêntica a senha"
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element) {
            $(element).removeClass('is-invalid');
        }
    });
});
