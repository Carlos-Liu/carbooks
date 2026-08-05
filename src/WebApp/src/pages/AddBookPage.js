import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Body1, Button, Dropdown, Field, Input, MessageBar, MessageBarBody, MessageBarTitle, Option, Spinner, Textarea, Title2, makeStyles, tokens, } from '@fluentui/react-components';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { ApiError } from '../api/client';
import { categoriesQuery, createBook } from '../api/catalog';
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
    const queryClient = useQueryClient();
    const { data: categories, isPending: categoriesPending, error: categoriesError, } = useQuery(categoriesQuery());
    const [name, setName] = useState('');
    const [author, setAuthor] = useState('');
    const [translator, setTranslator] = useState('');
    const [publisher, setPublisher] = useState('');
    const [publishedOn, setPublishedOn] = useState('');
    const [recommendation, setRecommendation] = useState('');
    const [isbn, setIsbn] = useState('');
    const [coverUrl, setCoverUrl] = useState('');
    const [coverImage, setCoverImage] = useState(null);
    const [previewUrl, setPreviewUrl] = useState(null);
    const [selectedCategoryIds, setSelectedCategoryIds] = useState([]);
    useEffect(() => {
        return () => {
            if (previewUrl) {
                URL.revokeObjectURL(previewUrl);
            }
        };
    }, [previewUrl]);
    const mutation = useMutation({
        mutationFn: (input) => createBook(input),
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: ['categories'] });
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
            translator: translator.trim() || undefined,
            publisher: publisher.trim() || undefined,
            publishedOn: publishedOn || undefined,
            recommendation: recommendation.trim() || undefined,
            isbn: isbn.trim() || undefined,
            coverUrl: coverUrl.trim() || undefined,
            coverImage,
            categoryIds: selectedCategoryIds,
        });
    };
    const selectedCategoryNames = (categories ?? [])
        .filter((category) => selectedCategoryIds.includes(category.id))
        .map((category) => category.name)
        .join(', ');
    const errorMessage = mutation.error instanceof ApiError || mutation.error instanceof Error
        ? mutation.error.message
        : mutation.error
            ? 'Could not create the book.'
            : null;
    return (_jsxs("section", { children: [_jsxs("div", { className: styles.header, children: [_jsx(AppLink, { to: "/", children: "Back to categories" }), _jsx(Title2, { children: "Add a book" }), _jsx(Body1, { children: "Create a catalog entry and optionally assign categories and a cover image." })] }), _jsxs("form", { className: styles.form, onSubmit: onSubmit, children: [_jsx(Field, { label: "Name", required: true, children: _jsx(Input, { value: name, onChange: (_, data) => setName(data.value), maxLength: 64, required: true }) }), _jsx(Field, { label: "Author", required: true, children: _jsx(Input, { value: author, onChange: (_, data) => setAuthor(data.value), maxLength: 64, required: true }) }), _jsx(Field, { label: "Translator", children: _jsx(Input, { value: translator, onChange: (_, data) => setTranslator(data.value), maxLength: 64 }) }), _jsx(Field, { label: "Publisher", children: _jsx(Input, { value: publisher, onChange: (_, data) => setPublisher(data.value), maxLength: 32 }) }), _jsx(Field, { label: "Published on", hint: "Date only, no time.", children: _jsx(Input, { type: "date", value: publishedOn, onChange: (_, data) => setPublishedOn(data.value) }) }), _jsx(Field, { label: "Recommendation", children: _jsx(Textarea, { value: recommendation, onChange: (_, data) => setRecommendation(data.value), maxLength: 1024, rows: 4 }) }), _jsx(Field, { label: "ISBN", hint: "Optional. Up to 32 characters.", children: _jsx(Input, { value: isbn, onChange: (_, data) => setIsbn(data.value), maxLength: 32 }) }), _jsx(Field, { label: "Categories", hint: "Optional. Select one or more categories. Names are shown; IDs are sent to the API.", children: categoriesPending ? (_jsx(Spinner, { size: "tiny", label: "Loading categories\u2026" })) : categoriesError ? (_jsx(MessageBar, { intent: "error", children: _jsxs(MessageBarBody, { children: [_jsx(MessageBarTitle, { children: "Could not load categories" }), categoriesError instanceof Error ? categoriesError.message : 'Unexpected error.'] }) })) : (_jsx(Dropdown, { multiselect: true, placeholder: "Select categories", selectedOptions: selectedCategoryIds, value: selectedCategoryNames, onOptionSelect: (_, data) => setSelectedCategoryIds(data.selectedOptions), children: (categories ?? []).map((category) => (_jsx(Option, { value: category.id, text: category.name, children: category.name }, category.id))) })) }), _jsx(Field, { label: "Cover URL", hint: "Optional absolute http(s) URL of the publisher artwork.", children: _jsx(Input, { type: "url", value: coverUrl, onChange: (_, data) => setCoverUrl(data.value), maxLength: 2048 }) }), _jsx(Field, { label: "Cover image", hint: "Optional. JPEG, PNG, GIF, WebP or SVG, up to 5 MB.", children: _jsx("input", { type: "file", accept: "image/jpeg,image/png,image/gif,image/webp,image/svg+xml", onChange: (event) => onCoverImageChange(event.target.files) }) }), previewUrl ? (_jsx("img", { className: styles.preview, src: previewUrl, alt: "Selected cover preview" })) : null, errorMessage ? (_jsx(MessageBar, { intent: "error", children: _jsxs(MessageBarBody, { children: [_jsx(MessageBarTitle, { children: "Could not add the book" }), errorMessage] }) })) : null, _jsxs("div", { className: styles.actions, children: [_jsx(Button, { type: "submit", appearance: "primary", disabled: mutation.isPending || categoriesPending, children: mutation.isPending ? 'Saving…' : 'Add book' }), _jsx(Button, { type: "button", appearance: "secondary", onClick: () => void navigate('/'), children: "Cancel" })] })] })] }));
}
