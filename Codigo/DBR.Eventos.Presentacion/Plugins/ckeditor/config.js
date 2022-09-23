/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */
CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config
    config.plugins =
		'about,' +
		'a11yhelp,' +
		'basicstyles,' +
		'blockquote,' +
		'clipboard,' +
		'colorbutton,' +
		'colordialog,' +
		'copyformatting,' +
		'contextmenu,' +
		'dialogadvtab,' +
		'elementspath,' +
		'enterkey,' +
		'entities,' +
		'filebrowser,' +
		'find,' +
		'floatingspace,' +
		'font,' +
		'format,' +
		'horizontalrule,' +
		'htmlwriter,' +
		'image,' +
		'iframe,' +
		'indentlist,' +
		'indentblock,' +
		'justify,' +
		'link,' +
		'list,' +
		'liststyle,' +
		'magicline,' +
		'maximize,' +
		'pastefromword,' +
		'pastetext,' +
		'removeformat,' +
		'resize,' +
		'sourcearea,' +
		'specialchar,' +
		'stylescombo,' +
		'tab,' +
		'table,' +
		'tableselection,' +
		'tabletools,' +
		'toolbar,' +
		'undo,' +
		'uploadimage,' +
		'lineheight,' +
        'emoji,' +
		'wysiwygarea';
	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbarGroups = [
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
		{ name: 'links' },
		{ name: 'insert', groups: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'] },
		{ name: 'forms' },
		{ name: 'tools' },		
		{ name: 'others' },		
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
        '/',
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi','JustifyLeft' ] },
		{ name: 'styles' },
		{ name: 'colors' }
	];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript';
	config.extraPlugins = 'colorbutton';
	config.extraPlugins = 'emoji';
	config.extraPlugins = 'lineheight';

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';
	config.language = 'es';

	config.line_height = "1px;2px;3px;4px;5px;6px;7px;8px;9px;10px;11px;12px;13px;14px;15px;16px;17px;18px;19px;20px;21px;22px;23px;24px;25px;26px;27px;28px;29px;30px";
	config.enterMode = "";
};

// Toolbar configuration generated automatically by the editor based on config.toolbarGroups.
