import React, { useState, useEffect } from "react";
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField, Typography } from "@mui/material";
import axios, { AxiosError } from "axios";

interface EditUserModalProps {
    open: boolean;
    onClose: () => void;
    userInfo: UserInfoEdit | null;
    onUserInfoUpdate: (updatedUserInfo: UserInfoEdit) => void;
}

export interface UserInfoEdit {
    firstName: string | null;
    lastName: string | null;
    phoneNumber: string | null;
    pictureUrl: string | null;
    birthDate: string | null;
}

export const EditUserInfoModal = ({ open, onClose, userInfo, onUserInfoUpdate }: EditUserModalProps) => {
    const [updatedUserInfo, setUpdatedUserInfo] = useState<UserInfoEdit | null>(userInfo);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);

    const [response, setResponse] = useState<{ type: 'success' | 'error', message: string } | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    useEffect(() => {
        setUpdatedUserInfo(userInfo);
    }, [userInfo]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setUpdatedUserInfo((prev) => (prev ? { ...prev, [name]: value } : null));
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            setSelectedFile(e.target.files[0]);
        }
    };

    const handleSubmit = async () => {
        setIsLoading(true);
        try {
            if (updatedUserInfo) {
                const formData = new FormData();
                formData.append("FirstName", updatedUserInfo.firstName || "");
                formData.append("LastName", updatedUserInfo.lastName || "");
                formData.append("PhoneNumber", updatedUserInfo.phoneNumber || "");
                formData.append("BirthDate", updatedUserInfo.birthDate || "");
                if (selectedFile) {
                    formData.append("userPicture", selectedFile);
                }

                const response = await axios.post("user/update-personal-info", formData, {
                    headers: { "Content-Type": "multipart/form-data" },
                });

                if (response.status === 200) {
                    onUserInfoUpdate(response.data);
                    onClose();
                    setResponse({ type: 'success', message: 'Данные успешно изменены'})
                }
            }
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response && error.response.status == 400) {
                    let errorText = error.response.data[0];
                    setResponse({ type: 'error', message: errorText})
                } else{
                    setResponse({ type: 'error', message: `Ошибка на стороне клиента: ${error}`})
                }
            }
            else {
                console.error('Ошибка:', error);
            }
        }
        finally{
            setIsLoading(false);
        }
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Редактировать данные</DialogTitle>
            <DialogContent>
                <TextField
                    margin="dense"
                    label="Имя"
                    type="text"
                    fullWidth
                    variant="outlined"
                    name="firstName"
                    value={updatedUserInfo?.firstName || ""}
                    onChange={handleInputChange}
                />
                <TextField
                    margin="dense"
                    label="Фамилия"
                    type="text"
                    fullWidth
                    variant="outlined"
                    name="lastName"
                    value={updatedUserInfo?.lastName || ""}
                    onChange={handleInputChange}
                />
                <TextField
                    margin="dense"
                    label="Дата рождения"
                    type="date"
                    fullWidth
                    variant="outlined"
                    name="birthDate"
                    InputLabelProps={{
                        shrink: true,
                    }}
                    value={updatedUserInfo?.birthDate || ""}
                    onChange={handleInputChange}
                />
                <TextField
                    margin="dense"
                    label="Телефон"
                    type="text"
                    fullWidth
                    variant="outlined"
                    name="phoneNumber"
                    value={updatedUserInfo?.phoneNumber || ""}
                    onChange={handleInputChange}
                />
                <input type="file" accept="image/*" onChange={handleFileChange} style={{ marginTop: "1rem" }} />
                <Typography variant="body2" color="textSecondary" style={{ marginTop: "0.5rem" }}>
                    Загрузите файл для обновления аватарки
                </Typography>
            </DialogContent>
            {response && (
                <Alert severity={response.type} sx={{ margin: '20px 0' }}>
                    {response.message}
                </Alert>
            )}
            <DialogActions>
                <Button onClick={onClose}>Отмена</Button>
                <Button onClick={handleSubmit} variant="contained" color="primary" disabled={isLoading}>
                    Сохранить
                </Button>
            </DialogActions>
        </Dialog>
    );
};
