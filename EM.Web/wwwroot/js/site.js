document.getElementById('nomeInput')?.addEventListener('keydown', function (event) {
    if (this.selectionStart === 0 && event.key === ' ') {
        event.preventDefault();
    }
});

document.getElementById('cpfInput')?.addEventListener('input', function (event) {
    let value = this.value.replace(/\D/g, '');

    if (value.length > 3) value = value.substring(0, 3) + '.' + value.substring(3);
    if (value.length > 7) value = value.substring(0, 7) + '.' + value.substring(7);
    if (value.length > 11) value = value.substring(0, 11) + '-' + value.substring(11);

    this.value = value;
});