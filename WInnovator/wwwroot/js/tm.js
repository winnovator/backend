tinymce.init({
    selector: 'textarea',
    theme: 'silver',
    plugins: 'print preview fullpage paste importcss searchreplace autolink save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
    menubar: 'file edit view insert format tools table help',
    toolbar: 'undo redo | bold italic underline strikethrough | formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media link anchor codesample | ltr rtl',
    toolbar_sticky: true,
    image_advtab: true,
    image_caption: true,
    quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
    noneditable_noneditable_class: "mceNonEditable",
    toolbar_drawer: 'sliding',
    contextmenu: "link image imagetools table",
    removed_menuitems: 'newdocument restorelastdraft',
    paste_data_images: true,
    resize: false,
    branding: false
});
