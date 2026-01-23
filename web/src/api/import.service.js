import axiosInstance from "./createAxiosAuth";

export const importWalletRecords = async (file) => {
  const formData = new FormData();
  formData.append("file", file);

  const response = await axiosInstance.post("/import/ImportWalletRecords", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  return response.data;
};
