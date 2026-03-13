export interface User {
    id: number;
    username: string;
    firstName: string;
    lastname: string;
    email: string;
    image : string | null;
    token: string;
}

export interface RegisterData {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    image: string | null;
}

export interface RegisterResponse {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    image: string | null;
}