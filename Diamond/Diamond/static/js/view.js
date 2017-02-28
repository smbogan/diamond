
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

        var input = $(editingTD.find("input"));

        if (save) {
            window.controller.updateViewEntryValue(path, fieldName, input.val());
        }

        editingTD.off("blur");
        editingTD.empty();
        editingTD.removeClass("editing");

        var newRow = $(window.controller.getRenderedViewEntry(path, fieldName));

        editingTD.parent().replaceWith(newRow);
    }

    var activateInputText = function (target) {
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
});