
$(function () {
    var path = $(".view-table").attr("view-path");

    $(document).keyup(function (e) {
        if (e.keyCode == 27) { //escape
            exitEditing(false);
        }
    });

    var exitEditing = function (save) {
        var editingTD = $(".editing");

        if(editingTD.is(".input-text-field"))
        {
            exitInputTextEditing(save);
        }
    }

    var exitInputTextEditing = function (save) {
        var editingTD = $(".editing");

        var fieldName = editingTD.attr('field-name');
        var variableName = editingTD.parent().attr('variable-name');

        var input = $(editingTD.find("input"));

        var reloadRows = [];

        if (save) {
            window.controller.updateViewEntryValue(path, fieldName, input.val());

            reloadRows = window.controller.getDependents(path, fieldName);
        }

        editingTD.off("blur");
        editingTD.empty();
        editingTD.removeClass("editing");

        var newRow = $(window.controller.getRenderedViewEntry(path, variableName));

        editingTD.parent().replaceWith(newRow);

        $.each(reloadRows, function (index, value) {
            console.log(value);
            $('tr[variable-name="' + value + '"]').replaceWith($(window.controller.getRenderedViewEntry(path, value)));
        });
    }

    var activateInputText = function (target) {
        console.log(target);

        if (target.prop("tagName").toLowerCase() != "td") {
            return;
        }

        var fieldName = target.attr("field-name");

        if (!target.is(".editing")) {
            var rawData = window.controller.getRawViewEntry(path, fieldName);

            var e = $('<input type="text" style="box-sizing: border-box; width: 100%;"></input>');
            e.val(rawData);

            target.empty();
            target.append(e);

            e.on("blur", function (event) {
                exitInputTextEditing(true);
            });

            e.keydown(function (event) {

                if (event.which === 13) { //enter
                    e.blur();
                }
            });

            e.focus();

            target.addClass("editing");
        }
    };

    $(document).on("click", ".input-text-field", function (event) {
        var e = $(event.target);
        activateInputText(e);
    });

    $(".save-view-button").click(function () {
        window.controller.saveView(path);
    });

    $(".create-table-button").click(function (event) {
        var tablePath = $(event.target).attr("table-path");

        var variableName = $(event.target).parent().parent().attr("variable-name");

        window.controller.createTable(path, variableName, tablePath);

        window.location.href = tablePath;
    });

    $(".edit-table-button").click(function (event) {
        var tablePath = $(event.target).attr("table-path");

        window.location.href = tablePath;
    });

    ///////
    $('.view-table').on('click', ".create-view-button", function (event) {
        var viewPath = $(event.target).attr("view-path");

        var variableName = $(event.target).parent().parent().attr("variable-name");

        window.controller.createView(path, variableName, viewPath);

        window.location.href = viewPath;
    });

    $('.view-table').on('click', ".edit-view-button", function (event) {
        var viewPath = $(event.target).attr("view-path");

        window.location.href = viewPath;
    });
});