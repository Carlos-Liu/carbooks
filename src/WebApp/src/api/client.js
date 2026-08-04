/**
 * Requests always go to a relative `/api` path. During development Vite proxies it to the API, and
 * in Docker Compose Nginx does the same, so the browser never makes a cross-origin call.
 */
const apiBasePath = '/api';
export class ApiError extends Error {
    status;
    constructor(status, message) {
        super(message);
        this.status = status;
        this.name = 'ApiError';
    }
}
export async function getJson(path, signal) {
    const response = await fetch(`${apiBasePath}${path}`, {
        headers: { Accept: 'application/json' },
        signal,
    });
    if (!response.ok) {
        throw new ApiError(response.status, await readProblemDetail(response));
    }
    return (await response.json());
}
/** Posts multipart form data (for example a book create with an optional image file). */
export async function postForm(path, formData, signal) {
    const response = await fetch(`${apiBasePath}${path}`, {
        method: 'POST',
        headers: { Accept: 'application/json' },
        body: formData,
        signal,
    });
    if (!response.ok) {
        throw new ApiError(response.status, await readProblemDetail(response));
    }
    return (await response.json());
}
/** Pulls the message out of an RFC 7807 payload, falling back to the status text. */
async function readProblemDetail(response) {
    try {
        const problem = (await response.json());
        return problem.detail ?? problem.title ?? response.statusText;
    }
    catch {
        return response.statusText || `Request failed with status ${response.status}.`;
    }
}
