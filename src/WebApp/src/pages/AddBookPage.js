import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Body1, Button, Field, Input, MessageBar, MessageBarBody, MessageBarTitle, Title2, makeStyles, tokens, } from '@fluentui/react-components';
import { useMutation } from '@tanstack/react-query';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { ApiError } from '../api/client';
import { createBook } from '../api/catalog';
import { AppLink } from '../components/AppLink';
const useStyles = makeStyles({
    header: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalXS,
        marginBottom: tokens.spacingVerticalXL,
    },
    form: {
        display: 'flex',
        flexDirection: 'column',
        rowGap: tokens.spacingVerticalL,
        maxWidth: '480px',
    },
    actions: {
        display: 'flex',
        columnGap: tokens.spacingHorizontalM,
        alignItems: 'center',
    },
    preview: {
        maxWidth: '200px',
        maxHeight: '260px',
        objectFit: 'contain',
        backgroundColor: tokens.colorNeutralBackground3,
    },
});
export function AddBookPage() {
    const styles = useStyles();
    const navigate = useNavigate();
    const [name, setName] = useState('');
    const [author, setAuthor] = useState('');
    const [coverUrl, setCoverUrl] = useState('');
    const [coverImage, setCoverImage] = useState(null);
    const [previewUrl, setPreviewUrl] = useState(null);
    useEffect(() => {
        return () => {
            if (previewUrl) {
                URL.revokeObjectURL(previewUrl);
            }
        };
    }, [previewUrl]);
    const mutation = useMutation({
        mutationFn: (input) => createBook(input),
        onSuccess: () => {
            void navigate('/');
        },
    });
    const onCoverImageChange = (fileList) => {
        const file = fileList?.[0] ?? null;
        setCoverImage(file);
        if (previewUrl) {
            URL.revokeObjectURL(previewUrl);
        }
        setPreviewUrl(file ? URL.createObjectURL(file) : null);
    };
    const onSubmit = (event) => {
        event.preventDefault();
        mutation.mutate({
            name: name.trim(),
            author: author.trim(),
            coverUrl: coverUrl.trim(),
            coverImage,
        });
    };
    const errorMessage = mutation.error instanceof ApiError || mutation.error instanceof Error
        ? mutation.error.message
        : mutation.error
            ? 'Could not create the book.'
            : null;
    return (_jsxs("section", { children: [_jsxs("div", { className: styles.header, children: [_jsx(AppLink, { to: "/", children: "Back to categories" }), _jsx(Title2, { children: "Add a book" }), _jsx(Body1, { children: "Create a catalog entry and optionally upload a local cover image." })] }), _jsxs("form", { className: styles.form, onSubmit: onSubmit, children: [_jsx(Field, { label: "Name", required: true, children: _jsx(Input, { value: name, onChange: (_, data) => setName(data.value), maxLength: 64, required: true }) }), _jsx(Field, { label: "Author", required: true, children: _jsx(Input, { value: author, onChange: (_, data) => setAuthor(data.value), maxLength: 64, required: true }) }), _jsx(Field, { label: "Cover URL", required: true, hint: "Absolute http(s) URL of the publisher artwork.", children: _jsx(Input, { type: "url", value: coverUrl, onChange: (_, data) => setCoverUrl(data.value), maxLength: 2048, required: true }) }), _jsx(Field, { label: "Cover image", hint: "Optional. JPEG, PNG, GIF, WebP or SVG, up to 5 MB.", children: _jsx("input", { type: "file", accept: "image/jpeg,image/png,image/gif,image/webp,image/svg+xml", onChange: (event) => onCoverImageChange(event.target.files) }) }), previewUrl ? (_jsx("img", { className: styles.preview, src: previewUrl, alt: "Selected cover preview" })) : null, errorMessage ? (_jsx(MessageBar, { intent: "error", children: _jsxs(MessageBarBody, { children: [_jsx(MessageBarTitle, { children: "Could not add the book" }), errorMessage] }) })) : null, _jsxs("div", { className: styles.actions, children: [_jsx(Button, { type: "submit", appearance: "primary", disabled: mutation.isPending, children: mutation.isPending ? 'Saving…' : 'Add book' }), _jsx(Button, { type: "button", appearance: "secondary", onClick: () => void navigate('/'), children: "Cancel" })] })] })] }));
}
