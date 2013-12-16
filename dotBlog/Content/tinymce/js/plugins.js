tinymce.PluginManager.add('csharp', function (editor, url) {
  editor.addButton('csharp', {
    text: 'C#',
    icon: false,
    onclick: function () {
      editor.focus(); 
      editor.selection.setContent('<pre class="brush: csharp">' + editor.selection.getContent() + '</pre>');
    }
  });
});