export declare class ApiError extends Error {
    readonly status: number;
    constructor(status: number, message: string);
}
export declare function getJson<T>(path: string, signal?: AbortSignal): Promise<T>;
/** Posts multipart form data (for example a book create with an optional image file). */
export declare function postForm<T>(path: string, formData: FormData, signal?: AbortSignal): Promise<T>;
