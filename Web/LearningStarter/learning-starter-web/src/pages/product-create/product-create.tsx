import { Button, Container, Input, Space, Text } from "@mantine/core";
import { useForm } from "@mantine/form";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import {
  ApiResponse,
  ProductCreateDto,
  ProductGetDto,
} from "../../constants/types";
import { ProductFormFields, ProductFormProvider, useProductForm } from "../products/product-form-fields";

export const ProductCreate = () => {
  const mantineForm = useProductForm({
    initialValues: {
      name: "",
      description: "",
    },
  });

  const submitProduct = async (data: ProductCreateDto) => {
    const result = await api.post<ApiResponse<ProductGetDto>>(
      "/api/products",
      data
    );

    if (result.data.hasErrors) {
      showNotification({ message: "Did Not Create Product!", color: "red" });
    }

    if (result.data.data) {
      showNotification({
        message: "Successfully Create Product!",
        color: "green",
      });
      
    }
  };

  return (
    <ProductFormProvider form={mantineForm}>
      <form onSubmit={mantineForm.onSubmit(submitProduct)}>
        <ProductFormFields />
        <Space h={10} />
          <Container px={0}>
            <Button type="submit">Create</Button>
          </Container>
      </form>
    </ProductFormProvider>
  );
};


